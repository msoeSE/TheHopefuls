var toTest = require(`${process.env.PWD}/app/libs/objectFunctions`);

describe("Object Functions Tests", function() {
	describe("MissingProperties", function(){
		it("should return no missing properties when object has all properties", function(){
			var missing = toTest.MissingProperties({"Property":true, "A": "String"}, ["Property", "A"]);
			expect(missing.length).toBe(0);
		});

		it("should return missing properties when the object doesn't have the required properties", function(){
			var missing = toTest.MissingProperties({"Property":true}, ["Property", "A"]);
			expect(missing.length).toBe(1);
			expect(missing).toContain("A");
		});

		it("should return undefined if it isn't an object being checked", function(){
			var missing = toTest.MissingProperties("Hello", ["Property", "A"]);
			expect(missing).toBe(undefined);
		});

		it("should return undefined if there isn't an array of properties to check", function(){
			var missing = toTest.MissingProperties({"Property": true}, "A");
			expect(missing).toBe(undefined);
		});
	});
});
