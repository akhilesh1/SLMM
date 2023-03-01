SLMM

*Assumption/Decesions*
--Making initial value of garden 5,3 which should be configured.
--Assuming SLMM has its own size as 1 Sq unit, so movement will be one block less.(Will discuss if needed in future)
--Made default Turn as right only if we not supply input.
--While SLMM is in progress while turning or forwarding 5/2 seconds 
if someone else make same request than then request should be rejected.(Achived using Moniter)
as we need our api to be responsive all the time
--Using Simple MS DI framework and MOQ for testing

*How to use*

build and run using visual studio or dotnet run

below are the API endpoints available which we can use by postman

GET  /Get   #for getting position
POST /Reset?length=<length>&width=<width>
POST /Turn?IsLeft=true
POST /Forward