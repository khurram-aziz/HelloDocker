#include <ESP8266WiFi.h>
#include <WiFiUDP.h>
#include <ESP8266mDNS.h>
#include <math.h>

int led = 0;
const char* mdnsName = "nodemcu";
// Replace with your settings
int port = 8888;                  //influxdb udp port
byte host[] = {192, 168, 0, 90};  //influxdb host ip
const char* ssid = "wifi-ssid";
const char* password = "wifi-key";

MDNSResponder mdns;
WiFiUDP udp; //WiFiClient client;
float temp = 0.0;

void setupWifi() {
  // Wait for connection and flash led till it connects
  // when connected; turn the led off
  while (WiFi.status() != WL_CONNECTED) {
    digitalWrite(led, HIGH);
    delay(200);
    digitalWrite(led, LOW);
    delay(200);
    Serial.print(".");
  }
  
  Serial.print("Connected to ");  Serial.println(ssid);
  Serial.print("IP address: ");   Serial.println(WiFi.localIP());

  // lets mDNS on the received ip
  if (mdns.begin(mdnsName, WiFi.localIP())) {
    Serial.print("mDNS started for "); Serial.print(mdnsName); Serial.println(".local");
  }
}

void setup() {
  pinMode(led, OUTPUT);
  digitalWrite(led, HIGH); //lets find out if its working
  WiFi.begin(ssid, password);
  Serial.begin(115200); 
  Serial.println("Setting up");
  
  setupWifi();

}
 
void loop() {
  if (WiFi.status() != WL_CONNECTED)
    setupWifi();
  int sensorValue = analogRead(0);
  float tempC = sensorValue * 3300.0 / 1023.0 / 10.0;
  float cValue = round(tempC * 10) / 10.0;
  
  if (cValue != temp) { //lets send only when its changed
    temp = cValue;
    
    String line = "temperature,device=" + String(mdnsName) + ",localip=" + String(WiFi.localIP()) + " value=" + String(temp, 2);
    udp.beginPacket(host, port);
    udp.print(line);
    udp.endPacket();
    
    //lets write whats send to serial for debugging and blink to physically device is working
    Serial.println(line);
    digitalWrite(led, HIGH);
    delay(200);
    digitalWrite(led, LOW);
    
    delay(5000); //we dont any new value for next 5sec atleast
  }
} 
