using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// API for general modules.
/// </summary>
namespace cl.uv.leikelen.API.Module.General
{
    /// <summary>
    /// General module that doesn't use the scene in use, 
    /// and can be enabled and used at any moment.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.AbstractModule" />
    public abstract class GeneralModule : AbstractModule
    {
    }
}
