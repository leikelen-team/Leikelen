using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.ProcessingModule
{
    public interface IProcessingModule
    {
        bool BeforeRecording();
        Action FunctionAfterStop();
    }
}
