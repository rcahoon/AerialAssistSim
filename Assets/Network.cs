using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Network : MonoBehaviour {
  public RobotController robot;
  public InputController teleop;
  private UdpClient udpClient;
  public int[] commands;
  private DateTime lastFeedback, lastCommand;
  private IAsyncResult receivePending;
  const int commandsPort = 7661;
  const int feedbackPort = 7662;
  
  void Awake() {
    commands = new int[0];
    receivePending = null;
    udpClient = new UdpClient(commandsPort);
    udpClient.Connect(IPAddress.Loopback, feedbackPort);
  }
  
  void OnDestroy() {
    if (udpClient != null) {
      udpClient.Close();
      udpClient = null;
    }
  }

  static void ReceiveCallback(IAsyncResult ar)
  {
    Network self = (Network)ar.AsyncState;
    
    if (self.receivePending == null) return;
    
    IPEndPoint e = new IPEndPoint(IPAddress.Any, commandsPort);
    Byte[] receiveBytes = self.udpClient.EndReceive(ar, ref e);
    if (receiveBytes.Length % sizeof(int) == 0) {
      int[] values = new int[receiveBytes.Length / sizeof(int)];
      Buffer.BlockCopy(receiveBytes, 0, values, 0, receiveBytes.Length);
      
      self.commands = values;
    }
    self.receivePending = null;
  }
  
  void Update() {
    if (DateTime.Now - lastFeedback > TimeSpan.FromMilliseconds(20)) {
      lastFeedback = DateTime.Now;
      int[] values = new int[] {
        robot.LeftEncoder,
        robot.RightEncoder,
        (int)robot.Heading,
        robot.GripperState ? 1 : -1,
        robot.BallPresence ? 1 : 0
      };
      byte[] sendBytes = new byte[values.Length * sizeof(int)];
      Buffer.BlockCopy(values, 0, sendBytes, 0, sendBytes.Length);
      /* string debug = "";
      foreach(var item in sendBytes)
        debug += item + " ";
      Debug.Log(debug); */
      udpClient.Send(sendBytes, sendBytes.Length);
    }

    if (receivePending == null) {
      if (commands.Length == 4) {
        teleop.enabled = false;
        robot.SetMotors(commands[0] / 512.0f, commands[1] / 512.0f);
        robot.SetGripper(commands[2] >= 0);
        if (commands[3] >= 256) {
          robot.Launch();
        }
      }
      
      receivePending = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this);
      lastCommand = DateTime.Now;
    } else if (DateTime.Now - lastCommand > TimeSpan.FromSeconds(1)) {
      OnDestroy();
      Awake();
      /*IPEndPoint e = new IPEndPoint(IPAddress.Any, commandsPort);
      IAsyncResult ar = receivePending;
      receivePending = null;
      try {
        udpClient.EndReceive(ar, ref e);
      } catch (Exception ex) {
        Debug.LogException(ex);
      }*/
      receivePending = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this);
      lastCommand = DateTime.Now;
      teleop.enabled = true;
    }
  }
}