using Grpc.Net.Client;

namespace Dolcecuore.Infrastructure.Grpc;

public class ChannelFactory
{
    // TODO: Verify the Certificate
    public static GrpcChannel Create(string address)
        => GrpcChannel.ForAddress(address);
}