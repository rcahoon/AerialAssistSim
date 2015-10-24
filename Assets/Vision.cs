using UnityEngine;
using System;
using System.Net;
using System.Threading;

public class Vision : MonoBehaviour {

  public Camera cam;
  
  private HttpListener listener = null;
  
  private byte[] image;

  public byte[] Capture() {
    RenderTexture currentRT = RenderTexture.active;
    RenderTexture.active = cam.targetTexture;
    cam.Render();
    Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
    image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
    image.Apply();
    RenderTexture.active = currentRT;
    return image.EncodeToJPG();
  }
  
  void Update()
  {
    if (image == null) {
      image = Capture();
    }
  }
  
  void Awake()
  {
    listener = new HttpListener();
    listener.Prefixes.Add("http://*:7663/");
    listener.Start();
    listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
  }
  
  void OnDestroy()
  {
    if (listener != null) {
      listener.Close();
      listener = null;
    }
  }
  
  void ListenerCallback(IAsyncResult result)
  {
    HttpListener listener = (HttpListener) result.AsyncState;
    // Call EndGetContext to complete the asynchronous operation.
    HttpListenerContext context = listener.EndGetContext(result);
    HttpListenerRequest request = context.Request;
    // Obtain a response object.
    HttpListenerResponse response = context.Response;
    response.ContentType = "image/jpeg";
    this.image = null;
    byte[] buffer = null;
    while (buffer == null) {
      buffer = this.image;
      Thread.Sleep(10);
    }
    // Get a response stream and write the response to it.
    response.ContentLength64 = buffer.Length;
    System.IO.Stream output = response.OutputStream;
    output.Write(buffer,0,buffer.Length);
    // You must close the output stream.
    output.Close();
    
    listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
  }
}