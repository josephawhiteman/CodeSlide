CodeSlide
=========

The implementation of this utility attempts to address the problem of
managing business logic in one code base whereby allowing different implementations to be generated yet allowing the code to run on multiple target systems or languages . This approach falls short of the overarching desire to build the runtime libraries directly for the target system bypassing the intermediate language or byte code used by resent languages which is then interpreted by JIT compilers. Google GWT makes a great attempt to drive the Client side javascript from the Java source code, thus having the business logic implemented in one place.

Code slide is a code generation utility utilizing XML and XSLT templates. The current Version is a Desktop App that allows for the configuration of project specific information I.E. Target Language, Pattern Implementation. This utility can read the database schema and generate the XML representation of the Entities and Relations between them.

It utilized Reflection in conjunction with Aspect Oriented programming to generate the UI. This pattern is similar to how frameworks like Spring and Hibernate map object properties to the database. Currently this implementation has the attribute connotations embedded in the Entity object source which at some time should be moved to an external XML configuration file.

Modified June 14 2012
