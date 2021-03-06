﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IModalAccess
    {
        List<ModalType> GetAll();
        ModalType Add(string name, string description);
        ModalType AddIfNotExists(string name, string description);
        bool Exists(string name);
    }
}
