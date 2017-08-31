using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Access.External;

namespace cl.uv.leikelen.ProcessingModule.test
{
    public class AccessData
    {
        AccessData()
        {
            var eventAccess = new EventAccess();
            //obtengo los eventos (puntos en el tiempo, en este caso no tienen valor,
            //pero es para marcar "estuvo")
            var events = eventAccess.GetAll(1, "Emotions", "LVHA");

            var modals = new ModalAccess().GetAll();
            foreach(var m in modals)
            {
                //Postura, emocion, voz, etc
                var submodals = new SubModalAccess().GetAll(m.Name);
                foreach(var s in submodals)
                {


                    //de postura: sentado, parado, brazos cruzados
                    var intervals = new IntervalAccess().GetAll(4, m.Name, s.Name);
                    var arrays = new TimelessAccess().GetAll(4, m.Name, s.Name);
                    var allEvents = new EventAccess().GetAll
                    foreach (var i in intervals)
                    {
                        if(arrays == null)
                        {

                        }
                        if(intervals != null)
                            Console.WriteLine($"El intervalo que empieza en {i.StartTime}, termina {i.EndTime}, tiene subtitulo {i.Subtitle}");
                    }
                }
            }
        }
    }
}
