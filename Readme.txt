1. Plug in Microbit
2. Go to Device Manager -> Ports (COM & LPT), find the "serial usb device COM", and note down the number behind "COM"
3. Right click on "serial usb device" and go to properties -> port settings and set "bits per second" to 115200
4. Download the ardity package here: https://ardity.dwilches.com/
5. Import the ardity package into Unity
6. In Unity, go to Edit -> Project Settings -> Player, and under "Other Settings" find an option that reads "Api Compatibility Level" and change it from ".NET 2.1" to ".NET 2.0".
7. Import my package into the project.
7. Add a SerialController prefab to the scene.
8. Adjust the serial controller:
