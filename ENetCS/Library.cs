﻿#region License
/*
ENet for C#
Copyright (c) 2011-2013 James F. Bellinger <jfb@zer7.com>

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
using ENet.Native;

namespace ENet
{
    /// <summary>
    /// Provides initialization, deinitialization, and time-keeping methods.
    /// </summary>
    public unsafe static class Library
    {
        /// <summary>
        /// Throws an exception if the ENet native library cannot be loaded.
        /// ENet is now automatically initialized, so it is no longer strictly
        /// necessary to call this function.
        /// </summary>
        /// <exception cref="ENetException">The native library cannot be loaded.</exception>
        public static void Initialize()
        {
            ENetApi.enet_time_get();
        }

        /// <summary>
        /// This method is retained for backwards compatibility. It does nothing.
        /// </summary>
        public static void Deinitialize()
        {

        }

        /// <summary>
        /// Gets or set the time in milliseconds.
        /// </summary>
        public static int Time
        {
            get { return (int)ENetApi.enet_time_get(); }
            set { ENetApi.enet_time_set((uint)value); }
        }
    }
}
