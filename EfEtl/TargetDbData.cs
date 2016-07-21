using EfEtl.Models;
using System;
using System.Collections.Generic;

namespace EfEtl
{
    /// <summary>
    /// Global singleton with TargetDbData organised in convenient / efficient format.
    /// Initialise once on startup, then use wherever.
    /// </summary>
    public class TargetDbData
    {
        public Dictionary<string, PropertyType> PropertyTypes { get; private set; }

        private static TargetDbData _instance = null;

        public static TargetDbData GetInstance()
        {
            if (_instance == null)
                throw new InvalidOperationException("Must call Initialise before using GetInstance");
            return _instance;
        }

        public static TargetDbData Initialise(EtlSpeedTestsEntities context)
        {
            if (_instance == null)
                _instance = new TargetDbData(context);
            return _instance;
        }

        private TargetDbData(EtlSpeedTestsEntities context)
        {
            PropertyTypes = new Dictionary<string, PropertyType>();
            foreach (var pt in context.PropertyTypes)
            {
                PropertyTypes[pt.Value] = pt;
            }
        }
    }
}
