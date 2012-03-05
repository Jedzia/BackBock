namespace Mono.XBuild.Utilities
{
    using System;

    public class MonoTODO : Attribute
    {
        private string msg;

        public MonoTODO(string msg)
        {
            this.msg = msg;
        }

        public MonoTODO()
        {
            this.msg = string.Empty;
        }
    }
}

/*namespace System
{
    public class MonoTODO : Attribute
    {
    }
}*/