using Braintree;
using Microsoft.Extensions.Options;

namespace ShoppingCar.Utilidades.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        public BrainTreeSettings _options { get; set; }
        private IBraintreeGateway brainTreeGateway { get; set; }

        public BrainTreeGate(IOptions<BrainTreeSettings> options)
        {
            _options = options.Value;
        }

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(_options.Enviroment, _options.MerchanId, _options.PublicKey, _options.PrivateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            if (brainTreeGateway == null)
                brainTreeGateway = CreateGateway();

            return brainTreeGateway;
        }
    }
}
