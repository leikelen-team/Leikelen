using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.Module
{
    public interface IModule
    {
        bool BeforeRecording();
        Action FunctionAfterStop();
    }
}
