using System.Collections;
using System.Numerics;

namespace Meetline.Modules.SharedKernel.Domain.Wrappers;

/// <summary>
///     Represents a set of permissions. This class is immutable and operations clone the PermissionSet.
/// </summary>
public readonly record struct PermissionSet
{
    public static readonly PermissionSet None = new(BigInteger.Zero);
    private readonly BigInteger _bits;

    private PermissionSet(BigInteger bits)
    {
        _bits = bits;
    }

    /// <summary>
    ///     Creates a PermissionSet from a byte array.
    /// </summary>
    public static PermissionSet FromBytes(byte[] bytes)
    {
        return new PermissionSet(new BigInteger(bytes, true));
    }

    /// <summary>
    ///     Returns the permission set as a byte array.
    /// </summary>
    public byte[] ToByteArray()
    {
        return _bits.ToByteArray(true);
    }

    /// <summary>
    ///     Add a new permission to the PermissionSet
    /// </summary>
    /// <param name="bitIndex">The permission's bit index to add</param>
    public PermissionSet Add(int bitIndex)
    {
        return new PermissionSet(_bits | (BigInteger.One << bitIndex));
    }

    /// <summary>
    ///     Remove a permission from the PermissionSet
    /// </summary>
    /// <param name="bitIndex">The permission's bit index to remove</param>
    public PermissionSet Remove(int bitIndex)
    {
        return new PermissionSet(_bits & ~(BigInteger.One << bitIndex));
    }

    /// <summary>
    ///     Returns whether the PermissionSet has a permission bit set.
    /// </summary>
    /// <param name="bitIndex">The permission's bit index whose presence to check</param>
    /// <returns>Whether the PermissionSet contains the permission given by that</returns>
    public bool Has(int bitIndex)
    {
        return !(_bits & (BigInteger.One << bitIndex)).IsZero;
    }

    public static PermissionSet operator |(PermissionSet left, PermissionSet right)
    {
        return new PermissionSet(left._bits | right._bits);
    }

    public static PermissionSet operator &(PermissionSet left, PermissionSet right)
    {
        return new PermissionSet(left._bits & right._bits);
    }

    public static PermissionSet operator ^(PermissionSet left, PermissionSet right)
    {
        return new PermissionSet(left._bits ^ right._bits);
    }

    public static PermissionSet operator ~(PermissionSet set)
    {
        return new PermissionSet(~set._bits);
    }

    /// <summary>
    ///     Returns a BitArray representation for EF Core compatibility.
    /// </summary>
    public BitArray ToBitArray()
    {
        var bytes = ToByteArray();
        return new BitArray(bytes);
    }

    /// <summary>
    ///     Creates a PermissionSet from a BitArray.
    /// </summary>
    public static PermissionSet FromBitArray(BitArray bits)
    {
        var bytes = new byte[(bits.Length + 7) / 8];
        bits.CopyTo(bytes, 0);
        return FromBytes(bytes);
    }
}