import { test, suite, suiteSetup } from 'mocha';
import * as vscode from "vscode";
import { activateExtension, assertContains, assertNone, assertNotContains, assertSize, assertStringContains, forServerToBeReady } from './testUtil';
import { complete, completeWithData, removeCheat } from './TestCompletionUtil';

suite(`CompletionProvider Types Tests`, () => {


    suiteSetup(async () => {
        await activateExtension();
        await forServerToBeReady();
    });

    test("Should show completions", async () => {
        let completionList = await complete("ShouldShow.cs", 1, 5);
        assertContains(completionList, "File");

    });

    test("Should not show completions when not needed", async () => {
        let [completionList, doc] = await completeWithData("ShouldNotShow.cs", 1, 4);
        assertNone(completionList.items, (completion) => completion.kind === vscode.CompletionItemKind.Reference);
    });

    test("Should filter out already used namespaces", async () => {
        let completionList = await complete("ShouldFilterOut.cs", 4, 4);
        assertNotContains(completionList, "File");
    });

    test("Should combine types of the same name", async () => {
        let [completionList] = await completeWithData("ShouldCombine.cs", 1, 6);
        completionList.items.sort((item1, item2) => item1.label.localeCompare(item2.label));
        let enumerables = completionList.items.filter(c => removeCheat(c.label) === "IEnumerable");
        assertSize(enumerables, 1);

        let enumerable = enumerables[0];

        assertStringContains(enumerable.detail!, "System.Collections");
        assertStringContains(enumerable.detail!, "System.Collections.Generic");
    });

    test("Should show types of a library", async () => {
        let completionList = await complete("ShouldShowLibraryType.cs", 4, 12);
        assertContains(completionList, "JsonConvert");
    });

    test("Should not show types of a not imported library", async () => {
        let completionList = await complete("ShouldNotShowOtherLibraryType.cs", 4, 12);
        assertNotContains(completionList, "MidiFile");
    });

    test("Should work despite being after a comment dot", async () => {
        let completionList = await complete("ShouldIgnoreComment.cs", 7, 8);
        assertContains(completionList, "Binder");
    });

    test("Should show completions even where there is no space before", async () => {
        let completionList = await complete("ShouldShowWithoutSpace.cs", 6, 20);
        assertContains(completionList, "File");
    });

    test("Should show completions after the 'this' keyword", async () => {
        let completionList = await complete("ShouldCompleteAfterThis.cs", 6, 37);
        assertContains(completionList, "IEnumerable");
    })

    test("Should add a using expression at the start of the document", async () => {
        let [completionList,document] = await completeWithData("ShouldAutoUsing.cs", 4, 11);
        assertContains(completionList.items.map(item => item.insertText), "List");
        assertStringContains(document.getText(),"using System.Collections.Generic") 
    })

    // TODO: test the completion adding the using expression.



});

