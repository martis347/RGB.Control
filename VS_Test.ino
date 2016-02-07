#define REDPIN 9  
#define GREENPIN 10
#define BLUEPIN 11
 

 
void setup()
{
  pinMode(REDPIN, OUTPUT);
  pinMode(GREENPIN, OUTPUT);
  pinMode(BLUEPIN, OUTPUT);
  pinMode(5, OUTPUT);
  Serial.begin(115200);
 
}
 byte RFin_bytes[3];
void loop()
{
  while (Serial.available()<3) {} 
  for(int n=0; n<3; n++)
    RFin_bytes[n] = Serial.read();  

    analogWrite(REDPIN,RFin_bytes[0]);
    analogWrite(GREENPIN,RFin_bytes[1]);
    analogWrite(BLUEPIN,RFin_bytes[2]);
  
}
