describe("Demo for testing", function () {
    it("Should return the true value", function () {
        var a = DemoForTest(2, 3);
        expect(a).toEqual(6);
        expect(a).not.toEqual(20);
    });
})