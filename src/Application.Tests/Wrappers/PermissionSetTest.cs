using Domain.Metadata;
using Domain.Wrappers;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Wrappers;

[TestSubject(typeof(PermissionSet))]
public class PermissionSetTest
{
    private static PermissionMetadata CreatePermission(int bitIndex)
    {
        return new PermissionMetadata(bitIndex, PermissionScope.Global);
    }

    [Fact(DisplayName = "Base case; should set the specific permission bit")]
    public void Add_BaseCase_ShouldSetPermissionBit()
    {
        var permissionSet = PermissionSet.None;
        var permission = CreatePermission(5);

        var result = permissionSet.Add(permission);

        Assert.True(result.Has(permission));
        Assert.False(permissionSet.Has(permission)); // To ensure immutability
    }

    [Fact(DisplayName = "Base case; should unset the specific permission bit")]
    public void Remove_BaseCase_ShouldUnsetPermissionBit()
    {
        var permissionSet = PermissionSet.None.Add(CreatePermission(5));
        var permission = CreatePermission(5);

        var result = permissionSet.Remove(permission);

        Assert.False(result.Has(permission));
        Assert.True(permissionSet.Has(permission));
    }

    [Fact(DisplayName = "Should return false for any nonexisting permissions")]
    public void Has_PermissionDoesNotExist_ShouldReturnFalse()
    {
        var permissionSet = PermissionSet.None;
        var permission = CreatePermission(5);

        var result = permissionSet.Has(permission);

        Assert.False(result);
    }

    [Fact(DisplayName = "Bitwise OR should combine permissions")]
    public void BitwiseOr_CombinesPermissions()
    {
        var p1 = CreatePermission(1);
        var p2 = CreatePermission(2);
        var s1 = PermissionSet.None.Add(p1);
        var s2 = PermissionSet.None.Add(p2);

        var result = s1 | s2;

        Assert.True(result.Has(p1));
        Assert.True(result.Has(p2));
    }

    [Fact(DisplayName = "Serialization should preserve bits")]
    public void Serialization_PreservesBits()
    {
        var p1 = CreatePermission(1);
        var p100 = CreatePermission(100);
        var original = PermissionSet.None.Add(p1).Add(p100);

        var bytes = original.ToByteArray();
        var deserialized = PermissionSet.FromBytes(bytes);

        Assert.Equal(original, deserialized);
        Assert.True(deserialized.Has(p1));
        Assert.True(deserialized.Has(p100));
    }
}