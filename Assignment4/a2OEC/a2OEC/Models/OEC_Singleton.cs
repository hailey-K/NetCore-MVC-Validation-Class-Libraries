using Microsoft.EntityFrameworkCore;
using a2OEC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace a2OEC.Models
{
    public class OEC_Singleton
    {
        private static OECContext _context;
        // generic object to used to exclude others
        private static object syncLock = new object();

        /// <summary>
        /// Instantiate the context instance, if it doesn't yet exist
        /// </summary>
        /// <returns>OECContext</returns>
        public static OECContext Context()
        {
            // Support multithreaded applications through 'double-checked locking':
            // - first program asking for the context locks everyone else out, then instantiates it
            // - when the lock is released, locked-out programs skip instantiation
            if (_context == null) // if instance already exists, skip to end
            {
                lock (syncLock) // first one here locks everyone else out until the instance is created
                {
                    if (_context == null) // people who were locked out now see instance & skip to end
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<OECContext>();
                        optionsBuilder.UseSqlServer(@"Server =.\sqlexpress;Database=OEC;Trusted_Connection=True");

                        _context = new OECContext(optionsBuilder.Options);
                    }
                }
            }
            return _context;
        }
    }
}
