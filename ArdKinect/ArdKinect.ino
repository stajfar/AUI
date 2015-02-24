#include <Adafruit_NeoPixel.h>

#define LEDLFACE       3

Adafruit_NeoPixel pixLFace = Adafruit_NeoPixel(1, LEDLFACE, NEO_GRB + NEO_KHZ800);

#define LEDRFACE       4

Adafruit_NeoPixel pixRFace = Adafruit_NeoPixel(1, LEDRFACE, NEO_GRB + NEO_KHZ800);

#define LEDBODY        5

#define NUMPIXELS      9

Adafruit_NeoPixel pixBody = Adafruit_NeoPixel(NUMPIXELS, LEDBODY, NEO_GRB + NEO_KHZ800);

#define FANWALL        7

#define COLORCODE      123

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

  pixLFace.begin(); // This initializes the NeoPixel library.
  pixRFace.begin(); // This initializes the NeoPixel library.
  pixBody.begin(); // This initializes the NeoPixel library.
  pinMode(FANWALL, OUTPUT);
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
              case LEDLFACE: //3
                ledLFace(inputByte_3, inputByte_4, inputByte_5);
              break;
              case LEDRFACE: //4
                ledRFace(inputByte_3, inputByte_4, inputByte_5);
              break;
              case LEDBODY: //5
                ledBody(inputByte_3, inputByte_4, inputByte_5);
              break;
              case FANWALL: //7
                if(inputByte_5 == 255)
                {
                  digitalWrite(FANWALL, HIGH); 
                }
                else
                {
                  digitalWrite(FANWALL, LOW); 
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

void ledLFace(int red, int green, int blu) {
    pixLFace.setPixelColor(0, pixLFace.Color(red, green, blu)); // Moderately bright green color.
    pixLFace.show(); // This sends the updated pixel color to the hardware.
    delay(1); // Delay for a period of time (in milliseconds).
}

void ledRFace(int red, int green, int blu) {
    pixRFace.setPixelColor(0, pixRFace.Color(red, green, blu)); // Moderately bright green color.
    pixRFace.show(); // This sends the updated pixel color to the hardware.
    delay(1); // Delay for a period of time (in milliseconds).
}

void ledBody(int red, int green, int blu) {
  // For a set of NeoPixels the first NeoPixel is 0, second is 1, all the way up to the count of pixels minus one.
  for(int i=0;i<NUMPIXELS;i++){
    // pixels.Color takes RGB values, from 0,0,0 up to 255,255,255
    pixBody.setPixelColor(i, pixBody.Color(red, green, blu)); // Moderately bright green color.
    pixBody.show(); // This sends the updated pixel color to the hardware.
    delay(1); // Delay for a period of time (in milliseconds).
  }
}
