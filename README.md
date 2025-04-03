# Spot-On: A Mixed Reality Interface for Multi-Robot Cooperation

**Supervisors**:  
Dr. Zuria Bauer (zuria.bauer@inf.ethz.ch)  
Dr. Hermann Blum (hermann.blum@inf.ethz.ch)  
MSc. Rene Zurbrugg (zrene@ethz.ch)  

**Authors**:  
Tim Engelbracht (tengelbracht@ethz.ch)  
Petar Lukovic (plukovic@ethz.ch)  
Kai Lascheit (klascheit@ethz.ch)  
Tjark Behrens (tbehrens@ethz.ch)


## Overview
Spot-On is an innovative Mixed Reality (MR) interface designed to facilitate collaboration among multiple quadruped robots in complex and semantically diverse environments. Using the Microsoft HoloLens 2, Spot-On provides an intuitive way to control robots via a 3D digital twin of the physical workspace.

<img src="images/spoton_pipeline_2.png" alt="Alt Text" width="1000">

## Key Features
- **Interactive Mixed Reality Interface**: Navigate and interact with a digital twin of the environment, enabling intuitive robot control.
- **Semantic Scene Graph**: Captures relationships between objects for robust task execution.
- **Collaborative Multi-Robot Tasks**: Supports tasks such as drawer handling, object manipulation, and light switch operation.
- **Voice Command Integration**: Simplifies interaction with the app.
- **Day/Night Modes**: Adapts the interface for different lighting conditions.

## Application Workflow
1. **Scene Initialization**: Scan the environment to create a digital twin using semantic instance segmentation.
2. **Task Assignment**: Use the HoloLens interface to interact with objects and issue tasks to robots.
3. **Task Execution**: Robots collaboratively perform tasks like fetching objects, opening drawers, or checking light states.
4. **Real-Time Feedback**: Updates in the scene graph are reflected on the HoloLens interface, ensuring a live synchronization with robot actions.

## Supported Tasks
### Single-Robot Tasks
- **Drawer Interaction**: Open and close drawers using precise trajectory computation.
- **Swing Door Interaction**: Detect and handle swing doors based on handle position.
- **Light Switch Operation**: Toggle light switches and verify lamp states.
- **Object Grasping**: Identify and manipulate objects using advanced grasping algorithms.
- **State Checking**: Inspect and report the status of objects (e.g., drawer open/closed, lamp on/off).

### Multi-Robot Tasks
- **Fetch & Drop**: One robot fetches an object and places it into the other robot’s basket.
- **Search & Drop**: A robot searches a container for a specific object and transfers it to its partner.
- **Operate & Check**: One robot operates light switches while the other verifies the state of connected lamps.

## User Study
Spot-On was validated through a user study involving participants with varied levels of experience in MR applications. Key findings include:
- High ratings for intuitiveness and design (average scores above 4.2/5).
- A steep learning curve, with users rapidly improving task completion times.
- Positive feedback on features like voice commands and day/night modes.

## Future Enhancements
- **Open-Vocabulary Voice Interface**: To allow more flexible voice interactions.
- **Dynamic Environment Mapping**: Real-time mapping using onboard depth sensors to eliminate manual setup.

---
For more information, please contact the authors or refer to the documentation provided with the application.

