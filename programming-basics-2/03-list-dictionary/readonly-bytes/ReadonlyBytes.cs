using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public static class FNVConstants
    {
        public const int OffsetBasis = unchecked((int)2166136261);
        public const int Prime = 16777619;
    }

    public class ReadonlyBytes : IEnumerable, IEnumerable<byte>
    {
        private readonly byte[] data;
        private bool isHaveHash;
        private int hash;
        public int Length { get { return data.Length; } }

        public ReadonlyBytes(params byte[] data)
        {
            if (data is null) throw new ArgumentNullException();
            this.data = data;
        }

        public byte this[int index]
        {
            get { return data[index]; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReadonlyBytes bytes) 
                || obj.GetType() != GetType()) return false;
            if (Length != bytes.Length) return false;
            for (var i = 0; i < Length; i++)
                if (data[i] != bytes[i]) return false;
            return true;
        }

        public override int GetHashCode()
        {
            if (isHaveHash) return hash;
            unchecked
            {
                hash = FNVConstants.OffsetBasis;
                foreach (var b in data)
                {
                    hash = hash ^ b.GetHashCode();
                    hash = hash * FNVConstants.Prime;
                }
            }
            isHaveHash = true;
            return hash;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");
            for (var i = 0; i < Length; i++)
            {
                builder.Append(data[i]);
                if (i != Length - 1) builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
                yield return data[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}