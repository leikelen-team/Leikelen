using cl.uv.leikelen.src.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cl.uv.leikelen.src.interfaces
{
    public interface IPerson
    {
        int         ListIndex { get; }
        int         PersonId { get; }
        long        TrackingId { get; }
        string      Name { get; }
        GenderEnum  Gender { get; }
        int         Age { get; }
        IScene      Scene { get; }
        Brush       Color { get; }
        bool        HasBeenTracked { get; }
        PersonView  View { get; }

        IHumanModal PostureModal { get; }
        //IHumanModal VoiceModal { get; }

        void generateView();

        //void generatePostureIntervals();
        //void generatePostureIntervals();
        //void addVoice();
        //void generateDistanceSum();
        //void generateDistanceEntropy();
        //void generateIntervalDistancesSum();
        //void generateIntervalDistancesEntropy();
    }
}
