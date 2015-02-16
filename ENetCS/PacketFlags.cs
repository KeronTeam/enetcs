﻿#region License
/*
ENet for C#
Copyright (c) 2011-2012 James F. Bellinger <jfb@zer7.com>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

using System;

namespace ENet
{
    /// <summary>
    /// Specifies outgoing packet transmittion behavior.
    /// </summary>
    [Flags]
    public enum PacketFlags
    {
        /// <summary>
        /// Sequence the packet, and unless it is larger than the MTU
        /// and requires fragmentation, send it unreliably.
        /// </summary>
        None = 0,

        /// <summary>
        /// Send the packet reliably.
        /// </summary>
        Reliable = 1 << 0,

        /// <summary>
        /// Allow the packet to arrive out-of-order.
        /// </summary>
        Unsequenced = 1 << 1,

        /// <summary>
        /// Let the application, not ENet, handle memory allocation for the packet.
        /// </summary>
        NoAllocate = 1 << 2,

        /// <summary>
        /// Even if an unreliable packet is larger than the MTU
        /// and requires fragmentation, send it unreliably.
        /// </summary>
        UnreliableFragment = 1 << 3
    }
}
