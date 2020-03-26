using System;
using Xunit;

namespace NngSharp.Tests
{
    public class NngExceptionTests
    {
        [Theory]
        [MemberData(nameof(GetErrorCodesAndDescriptions))]
        public void GetMessage(NngErrorCode errorCode, string expectedMessage)
        {
            // note: this test does not have any value except to figure out what NNG error code names stand for (as in NNG_ETRANERR)
            var exception = new NngException(errorCode);
            Assert.Equal(expectedMessage, exception.Message);
        }

        public static TheoryData<NngErrorCode, string> GetErrorCodesAndDescriptions()
        {
            var data = new TheoryData<NngErrorCode, string>();

            foreach (NngErrorCode errorCode in Enum.GetValues(typeof(NngErrorCode)))
            {
                string errorMessage;
                // note: should have used a dictionary but it's easier when the compiler generates the switch labels
                switch (errorCode)
                {
                    case NngErrorCode.Unknown:
                        errorMessage = "Unknown error";
                        break;
                    case NngErrorCode.Success:
                        errorMessage = "Hunky dory"; //LOL!
                        break;
                    case NngErrorCode.Interrupted:
                        errorMessage = "Interrupted";
                        break;
                    case NngErrorCode.NoMemory:
                        errorMessage = "Out of memory";
                        break;
                    case NngErrorCode.InvalidArgument:
                        errorMessage = "Invalid argument";
                        break;
                    case NngErrorCode.ResourceBusy:
                        errorMessage = "Resource busy";
                        break;
                    case NngErrorCode.TimedOut:
                        errorMessage = "Timed out";
                        break;
                    case NngErrorCode.ConnectionRefused:
                        errorMessage = "Connection refused";
                        break;
                    case NngErrorCode.Closed:
                        errorMessage = "Object closed";
                        break;
                    case NngErrorCode.TryAgain:
                        errorMessage = "Try again";
                        break;
                    case NngErrorCode.NotSupported:
                        errorMessage = "Not supported";
                        break;
                    case NngErrorCode.AddressInUse:
                        errorMessage = "Address in use";
                        break;
                    case NngErrorCode.IncorrectState:
                        errorMessage = "Incorrect state";
                        break;
                    case NngErrorCode.NoEntry:
                        errorMessage = "Entry not found";
                        break;
                    case NngErrorCode.ProtocolError:
                        errorMessage = "Protocol error";
                        break;
                    case NngErrorCode.DestinationUnreachable:
                        errorMessage = "Destination unreachable";
                        break;
                    case NngErrorCode.AddressInvalid:
                        errorMessage = "Address invalid";
                        break;
                    case NngErrorCode.PermissionDenied:
                        errorMessage = "Permission denied";
                        break;
                    case NngErrorCode.MessageSize:
                        errorMessage = "Message too large";
                        break;
                    case NngErrorCode.ConnectionAborted:
                        errorMessage = "Connection aborted";
                        break;
                    case NngErrorCode.ConnectionReset:
                        errorMessage = "Connection reset";
                        break;
                    case NngErrorCode.OperationCanceled:
                        errorMessage = "Operation canceled";
                        break;
                    case NngErrorCode.NoFiles:
                        errorMessage = "Out of files";
                        break;
                    case NngErrorCode.NoSpace:
                        errorMessage = "Out of space";
                        break;
                    case NngErrorCode.ExistsResource:
                        errorMessage = "Resource already exists";
                        break;
                    case NngErrorCode.ReadOnlyResource:
                        errorMessage = "Read only resource";
                        break;
                    case NngErrorCode.WriteOnlyResource:
                        errorMessage = "Write only resource";
                        break;
                    case NngErrorCode.CryptographicError:
                        errorMessage = "Cryptographic error";
                        break;
                    case NngErrorCode.PeerAuthenticationFailed:
                        errorMessage = "Peer could not be authenticated";
                        break;
                    case NngErrorCode.NoArguments:
                        errorMessage = "Option requires argument";
                        break;
                    case NngErrorCode.AmbiguousOption:
                        errorMessage = "Ambiguous option";
                        break;
                    case NngErrorCode.BadType:
                        errorMessage = "Incorrect type";
                        break;
                    case NngErrorCode.ConnectionShutdown:
                        errorMessage = "Unknown error #31";
                        break;
                    case NngErrorCode.InternalError:
                        errorMessage = "Internal error detected";
                        break;
                    case NngErrorCode.SystemError:
                        errorMessage = "No error";
                        break;
                    case NngErrorCode.TransportError:
                        errorMessage = "Transport error #0";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                data.Add(errorCode, errorMessage);
            }

            return data;
        }
    }
}