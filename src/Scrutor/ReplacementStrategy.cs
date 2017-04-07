﻿using System;

namespace Scrutor
{
    [Flags]
    public enum ReplacementStrategy
    {
        /// <summary>
        /// ServiceType only is the default
        /// </summary>
        Default = 0,
        /// <summary>
        /// Replace by ServiceType (default)
        /// </summary>
        ServiceType = 1,
        /// <summary>
        /// Replace by ImplementationType.
        /// </summary>
        ImplementationType = 2,
    }
}