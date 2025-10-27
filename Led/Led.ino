int ledPin = 12;

void setup() {
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600); // Must match C# baud rate
}

void loop() {
  if (Serial.available()) {
    char cmd = Serial.read();

    if (cmd == '1') {
      digitalWrite(ledPin, HIGH); // LED ON
    } 
    else if (cmd == '0') {
      digitalWrite(ledPin, LOW);  // LED OFF
    }
  }
}
