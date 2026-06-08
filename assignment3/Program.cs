Console.WriteLine("Hello, World!");
/*
 Q1: Identify the Type of Relationship
a) Composition – The lifetime of the Department is strictly bound to the University. If the university is destroyed, the departments cease to exist.

b) Association – A loose relationship where two independent objects (Driver and Car) interact, but neither owns or controls the lifetime of the other.

c) Inheritance – An "is-a" relationship where Dog is a specialized type of Animal.

d) Aggregation – A "has-a" relationship where the child (Player) can exist independently of the parent (Team).

e) Dependency – A transient "uses-a" relationship. The method relies on the Logger short-term, but does not maintain a permanent link or reference to it as a class field.

Q2: Access Modifiers and Sealed
a) * Yes, a child class in a different assembly can access the protected field, but only within its own class scope via inheritance (e.g., inside its own methods using this.protectedField).

No, you cannot access it through an object instance from the outside (e.g., childInstance.protectedField will cause a compilation error outside the class).

b) * protected internal: Accessible anywhere within the same assembly OR by derived classes in any assembly (an OR condition).

private protected: Accessible only by derived classes that are located within the same assembly (an AND condition).

c) * Applied to a class: Prevents other classes from inheriting from it.

Applied to a method: Prevents derived classes from overriding that specific method (can only be applied to methods that override a base virtual/abstract method).

d) Yes, you absolutely can. The sealed keyword only restricts inheritance (preventing a class from acting as a parent). It does not restrict instantiation, so you can freely use the new keyword to create instances of it.


 */



namespace MovieTicketBookingSystem
{
  
    public class Ticket
    {
        private static int _ticketCounter = 0;

        public int TicketId { get; private set; }
        public string MovieName { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than 0.");
                _price = value;
            }
        }

        public decimal PriceAfterTax => Price * 1.14m; // 14% Tax

        public Ticket(string movieName, decimal price)
        {
            _ticketCounter++;
            TicketId = _ticketCounter;
            MovieName = movieName;
            Price = price;
        }

        public static int GetTotalTickets()
        {
            return _ticketCounter;
        }

        public override string ToString()
        {
            return $"ID: {TicketId} | Movie: {MovieName} | Base Price: {Price:N2} EGP | Taxed Price: {PriceAfterTax:N2} EGP";
        }
    }

  
    public class StandardTicket : Ticket
    {
        public string SeatNumber { get; set; }

        public StandardTicket(string movieName, decimal price, string seatNumber)
            : base(movieName, price)
        {
            SeatNumber = seatNumber;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Type: Standard | Seat: {SeatNumber}";
        }
    }


    public class VIPTicket : Ticket
    {
        public bool LoungeAccess { get; set; }
        public decimal ServiceFee { get; } = 50.00m;

        
        public new decimal PriceAfterTax => base.PriceAfterTax + ServiceFee;

        public VIPTicket(string movieName, decimal price, bool loungeAccess)
            : base(movieName, price)
        {
            LoungeAccess = loungeAccess;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Type: VIP | Lounge Access: {(LoungeAccess ? "Yes" : "No")} | Total (inc. Service Fee): {PriceAfterTax:N2} EGP";
        }
    }

  
    public class IMAXTicket : Ticket
    {
        public bool Is3D { get; set; }

        public IMAXTicket(string movieName, decimal price, bool is3D)
            : base(movieName, is3D ? price + 30 : price) 
        {
            Is3D = is3D;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Type: IMAX | 3D: {(Is3D ? "Yes" : "No")}";
        }
    }

    
    public class Projector
    {
        public void Start() => Console.WriteLine("Projector is now ON and broadcasting.");
        public void Stop() => Console.WriteLine("Projector is now OFF.");
    }

    public class Cinema
    {
        public string CinemaName { get; set; }
        private Projector _projector; 
        private Ticket[] _tickets;
        private int _ticketCount;

        public Cinema(string cinemaName)
        {
            CinemaName = cinemaName;
            _projector = new Projector(); 
            _tickets = new Ticket[20];    
            _ticketCount = 0;
        }

        public void AddTicket(Ticket t)
        {
            if (_ticketCount < _tickets.Length)
            {
                _tickets[_ticketCount] = t;
                _ticketCount++;
                Console.WriteLine($"Ticket successfully added for: {t.MovieName}");
            }
            else
            {
                Console.WriteLine("Error: Cinema ticket capacity reached!");
            }
        }

        public void PrintAllTickets()
        {
            Console.WriteLine($"\n--- Current Tickets at {CinemaName} ---");
            if (_ticketCount == 0)
            {
                Console.WriteLine("No tickets sold yet.");
                return;
            }

            for (int i = 0; i < _ticketCount; i++)
            {
                Console.WriteLine(_tickets[i].ToString());
            }
            Console.WriteLine($"Total Tickets Extracted System-wide: {Ticket.GetTotalTickets()}");
            Console.WriteLine("---------------------------------------\n");
        }

        public void OpenCinema()
        {
            Console.WriteLine($"Opening {CinemaName}...");
            _projector.Start();
        }

        public void CloseCinema()
        {
            Console.WriteLine($"Closing {CinemaName}...");
            _projector.Stop();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
        
            Cinema myCinema = new Cinema("Grand IMAX Plaza");
            myCinema.OpenCinema();
            Console.WriteLine();

            StandardTicket ticket1 = new StandardTicket("Inception", 100.00m, "G12");
            VIPTicket ticket2 = new VIPTicket("Interstellar", 200.00m, true);
            IMAXTicket ticket3 = new IMAXTicket("Avatar: The Way of Water", 150.00m, true); 

            myCinema.AddTicket(ticket1);
            myCinema.AddTicket(ticket2);
            myCinema.AddTicket(ticket3);

            
            myCinema.PrintAllTickets();

         
            myCinema.CloseCinema();

            Console.ReadKey();
        }
    }
}