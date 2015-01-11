#include <Adafruit_NeoPixel.h>

#define LEDPIN         3

#define FANPIN         4

#define COLORCODE      123

#define NUMPIXELS      12

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, LEDPIN, NEO_GRB + NEO_KHZ800);

//Setup message bytes

byte inputByte_0;

byte inputByte_1;

byte inputByte_2;

byte inputByte_3;

byte inputByte_4;

byte inputByte_5;

byte inputByte_6;

//Setup

void setup() {

  pixels.begin(); // This initializes the NeoPixel library.
  pinMode(FANPIN, OUTPUT);
  ColorSetup();
  Serial.begin(9600);
}

//Main Loop

void loop() {
  //Read Buffer
  if (Serial.available() >= 7) 
  {
    //Read buffer
    inputByte_0 = Serial.read();
    inputByte_1 = Serial.read();
    inputByte_2 = Serial.read();
    inputByte_3 = Serial.read();
    inputByte_4 = Serial.read(); 
    inputByte_5 = Serial.read(); 
    inputByte_6 = Serial.read();   
  }
  //Check for start of Message
  if(inputByte_0 == 16)
  {
       //Detect Command type
       switch (inputByte_1) 
       {
          case 100:
             //Set PIN and value
             switch (inputByte_2)
            {
              case LEDPIN: //3
                led(inputByte_3, inputByte_4, inputByte_5);
              break;
              case FANPIN: //4
                if(inputByte_5 == 255)
                {
                  digitalWrite(FANPIN, HIGH); 
                }
                else
                {
                  digitalWrite(FANPIN, LOW); 
                }
              break;
            } 
            break;
          case 101:
             //Set PIN and value
             switch (inputByte_2)
            {
              case COLORCODE: //123
                ColorSerialPrint();
              break;
            } 
            break;
          case 128:
            //Say hello
            Serial.print("HELLO FROM ARDUINO");
            break;
        } 
        //Clear Message bytes
        inputByte_0 = 0;
        inputByte_1 = 0;
        inputByte_2 = 0;
        inputByte_3 = 0;
        inputByte_4 = 0;
        //Let the PC know we are ready for more data
        Serial.print("-READY TO RECEIVE");
  }
}

void led(int red, int green, int blu) {
  // For a set of NeoPixels the first NeoPixel is 0, second is 1, all the way up to the count of pixels minus one.
  for(int i=0;i<NUMPIXELS;i++){
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixels.setPixelColor(i, pixels.Color(red, green, blu)); // Moderately bright green color.
    pixels.show(); // This sends the updated pixel color to the hardware.
    delay(1); // Delay for a period of time (in milliseconds).
  }
}
