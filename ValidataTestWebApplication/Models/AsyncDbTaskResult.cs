using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    public class AsyncDbTaskResult
    {
        public AsyncDbTaskResult() { }

        /// <summary>
        /// Task result is returned from SaveChangesAsync
        /// </summary>
        public int TaskResult { get; set; }
    }

    public class AsyncDbTaskResultForCreate : AsyncDbTaskResult
    {
        public AsyncDbTaskResultForCreate() { }

        /// <summary>
        /// Id of newly added record to database
        /// </summary>
        public int NewId { get; set; }
    }
}