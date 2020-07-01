using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.API
{
    public abstract class Database
    {
        protected readonly object DbLock = new object();
        public Database() {}
    }
}
