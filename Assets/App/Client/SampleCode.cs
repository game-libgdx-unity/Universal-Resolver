using ABank;

namespace UnityIoC
{

    public class MyPayment
    {
        private CardProcessor TheCardProcessor { get; set; }

        public MyPayment(CardProcessor cardProcessor)
        {
            TheCardProcessor = cardProcessor;
        }
        public MyPayment()
        {
            TheCardProcessor = CardProcessor.SharedInstance;
        }
        
        public void DoSomeBusiness()
        {
            //do your business
        }
        public void DoSomeBusiness(Card card)
        {
            TheCardProcessor.Process(card);
        }
        
        

        public static void Main(string[] args)
        {
            //Singleton design pattern
            CardProcessor.Initialize();
            
            MyPayment payment = new MyPayment();
            payment.DoSomeBusiness();
            //--------
            
            
            
            
            
            
            //Dependency injection design
            MyPayment payment2 = new MyPayment(CardProcessor.Create());
            payment2.DoSomeBusiness();
            //---------
            
            //
            
        }
    }
    
   
}

namespace ABank
{
    public class CardProcessor
    {
        public static CardProcessor SharedInstance;

        public static void Initialize()
        {
        }

        public static CardProcessor Create()
        {
            return new CardProcessor();
        }
        
        public void Process(Card card)
        {
            
        }
    }

    public class Card
    {
        public int ID { get; set; }
    }
}