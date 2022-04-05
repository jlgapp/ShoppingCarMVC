using Braintree;

namespace ShoppingCar.Utilidades.BrainTree
{
	public interface IBrainTreeGate
	{
		IBraintreeGateway CreateGateway();
		IBraintreeGateway GetGateway();
	}
}
