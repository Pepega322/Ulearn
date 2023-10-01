using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                if (value < 0) throw new ArgumentException();
                price = value;
                Notify(nameof(Price));
                total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Total));
            }
        }
        private int nightsCount;
        public int NightsCount
        {
            get { return nightsCount; }
            set
            {
                if (value <= 0) throw new ArgumentException();
                nightsCount = value;
                Notify(nameof(NightsCount));
                total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Total));
            }
        }
        private double discount;
        public double Discount
        {
            get { return discount; }
            set
            {
                if (value > 100) throw new ArgumentException();
                discount = value;
                Notify(nameof(Discount));
                total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Total));
            }
        }
        private double total;
        public double Total
        {
            get { return total; }
            set
            {
                if (value < 0) throw new ArgumentException();
                total = value;
                Notify(nameof(Total));
                discount = (1 - total / (price * nightsCount)) * 100;
                Notify(nameof(Discount));
            }
        }
    }
}