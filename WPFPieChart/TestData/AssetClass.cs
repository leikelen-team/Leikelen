using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFPieChart
{
    public class AssetClass : INotifyPropertyChanged
    {
        private String myClass;

        public String Class
        {
            get { return myClass; }
            set {
                myClass = value;
                RaisePropertyChangeEvent("Class");
            }
        }

        private double fund;

        public double Fund
        {
            get { return fund; }
            set {
                fund = value;
                RaisePropertyChangeEvent("Fund");
            }
        }

        private double total;

        public double Total
        {
            get { return total; }
            set {
                total = value;
                RaisePropertyChangeEvent("Total");
            }
        }

        private double benchmark;

        public double Benchmark
        {
            get { return benchmark; }
            set {
                benchmark = value;
                RaisePropertyChangeEvent("Benchmark");
            }
        }



        public static List<AssetClass> ConstructTestData()
        {
            List<AssetClass> assetClasses = new List<AssetClass>();

            assetClasses.Add(new AssetClass(){Class="Cash", Fund=1.56, Total=1.56, Benchmark=4.82});
            assetClasses.Add(new AssetClass(){Class="Bonds", Fund=2.92, Total=2.92, Benchmark=17.91});
            assetClasses.Add(new AssetClass(){Class="Real Estate", Fund=13.24, Total=2.40, Benchmark=0.04});
            assetClasses.Add(new AssetClass(){Class="Foreign Currency", Fund=16.44, Total=16.44, Benchmark=8.05});
            assetClasses.Add(new AssetClass(){Class="Stocks; Domestic", Fund=27.57, Total=27.57, Benchmark=38.24});
            assetClasses.Add(new AssetClass(){Class="Stocks; Foreign", Fund=50.03, Total=50.03, Benchmark=30.93});

            return assetClasses;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangeEvent(String propertyName)
        {
            if (PropertyChanged!=null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }

        #endregion
    }
}
