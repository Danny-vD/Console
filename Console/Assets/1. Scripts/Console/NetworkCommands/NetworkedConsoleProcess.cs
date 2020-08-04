using System.Collections;
using Console.Attributes;
using Console.Properties;
using UnityEngine;

namespace Assets.Console.NetworkCommands
{
    public class NetworkedConsoleProcess : MonoBehaviour
    {
        [Range(0.001f, 1), ConsoleProperty("console.client.logpolling.tick")]
        public float ClientLogPollingTick = 0.1f;

        [Range(0.001f, 1), ConsoleProperty("console.host.commandprocess.tick")]
        public float HostProcessingTick = 0.1f;

        private HostConsoleCommand hc = new HostConsoleCommand();
        private ClientConsoleCommand cc = new ClientConsoleCommand();
        private IEnumerator Host;
        private IEnumerator Client;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            ConsolePropertyAttributeUtils.InitializePropertySystem();
            ConsolePropertyAttributeUtils.AddProperties(this);
            CommandAttributeUtils.AddCommands(hc);
            CommandAttributeUtils.AddCommands(cc);
            Host = HostRoutine();
            Client = ClientRoutine();
            StartCoroutine(Host);
            StartCoroutine(Client);
        }

        private void OnDestroy()
        {
            StopCoroutine(Host);
            StopCoroutine(Client);
        }

        private IEnumerator HostRoutine()
        {
            while (true)
            {
                HostConsoleCommand.ProcessQueue();
                yield return new WaitForSeconds(HostProcessingTick);
            }
        }

        private IEnumerator ClientRoutine()
        {
            while (true)
            {
                ClientConsoleCommand.ProcessLogMessages();
                yield return new WaitForSeconds(ClientLogPollingTick);
            }
        }
    }
}