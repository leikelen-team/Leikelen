﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface ISceneAccess
    {
        List<Scene> GetAll();
        Scene Get(int sceneId);
        bool Exists(int sceneId);
        Scene SaveOrUpdate(Scene scene);
        Scene SaveNew(Scene scene);
        void Delete(Scene scene);
    }
}
