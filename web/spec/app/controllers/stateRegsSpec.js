var StateRegulations = require("../app/models/StateRegulations.js");

describe("State Regulations Endpoint Tests", function () {
  var success;
  var failure;
  var shouldErr;
  var stateRegsMockObject = {"Object": "MockedSchool"};
  beforeEach(function(){
    shouldErr = undefined;
    success = jasmine.createSpy("success");
    failure = jasmine.createSpy("failure");
  });

  describe("getStateRegs", function() {
      beforeEach(function(){
          spyOn(StateRegulations, "create").andCallFake(function(_, callback){
              if(shouldErr){
                  callback("error", null);
              } else {
                  callback(null, stateRegsMockObject);
              }
          });
      });
      it();
  })
});
