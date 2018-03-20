using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.Processing.Kinect.Distance
{
    public class DistanceLogic
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();


        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
        }
    }
}
