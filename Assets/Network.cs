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
  
  // Command indexes
  const int RESET_SIM = 0;
  
	const int LEFT_MOTOR = 10;
	const int RIGHT_MOTOR = 11;
	const int INTAKE = 12;
	const int LAUNCH = 13;
  
  // Feedback indexes
  const int LEFT_ENCODER = 10;
  const int RIGHT_ENCODER = 11;
  const int HEADING = 12;
  const int INTAKE_STATE = 13;
  const int BALL_PRESENCE = 14;
  
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
      int[] values = new int[64];
      values[LEFT_ENCODER] = robot.LeftEncoder;
      values[RIGHT_ENCODER] = robot.RightEncoder;
      values[HEADING] = (int)robot.Heading;
      values[INTAKE_STATE] = robot.GripperState ? 1 : -1;
      values[BALL_PRESENCE] = robot.BallPresence ? 1 : 0;
      
      byte[] sendBytes = new byte[values.Length * sizeof(int)];
      Buffer.BlockCopy(values, 0, sendBytes, 0, sendBytes.Length);
      /* string debug = "";
      foreach(var item in sendBytes)
        debug += item + " ";
      Debug.Log(debug); */
      udpClient.Send(sendBytes, sendBytes.Length);
    }

    if (receivePending == null) {
      if (commands.Length >= 14) {
        if (commands[RESET_SIM] > 0) {
          Debug.Log("Reset");
          Application.LoadLevel(Application.loadedLevel);
        }
        
        teleop.enabled = false;
        robot.SetMotors(commands[LEFT_MOTOR] / 512.0f, commands[RIGHT_MOTOR] / 512.0f);
        robot.SetGripper(commands[INTAKE] >= 0);
        if (commands[LAUNCH] >= 256) {
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