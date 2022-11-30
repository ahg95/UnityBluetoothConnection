1. Plug in Microbit
2. Go to Device Manager -> Ports (COM & LPT), find the "serial usb device COM", and note down the number behind "COM"
3. Right click on "serial usb device" and go to properties -> port settings and set "bits per second" to 115200
4. Download the ardity package here: https://ardity.dwilches.com/
5. Import the ardity package into Unity
6. In Unity, go to Edit -> Project Settings -> Player, and under "Other Settings" find an option that reads "Api Compatibility Level" and change it from ".NET 2.1" to ".NET 2.0".
7. Import the package "NatureVRScentController" into the project.
8. Add a SerialController prefab to the scene.
9. Add an empty gameObject to the scene and add the NatureVRScentController script to it.
10. Add colliders to the scene that act as scent sources. For example, add a cylinder collider to each tree object that is the size of the stem.
11. Adjust the NatureVRScentController component: Set the "Serial Controller" to the serial controller gameObject you just added. Set the "Player Head" as the transform of the head of the player. Specify the colliders you just added in "Scent Source Colliders".
12. Adjust the SerialController gameObject: set the "Port Name" to the name of the serial usb device port that you noted down earlier (e.g., COM3). Set the "Baud Rate" to 115200. Set the "Message Listener" to the gameObject with the NatureVRScentController.
13. It should work if you hit play!