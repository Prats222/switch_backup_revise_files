# C# DSA Interview Tracker

## Live tracker

**[Open the hosted C# DSA Interview Tracker](https://csharp-dsa-interview-tracker.vercel.app)**

A responsive, single-file DSA tracker for early-career SDE/SDET interview preparation.

## Open the tracker

- [Open index.html in GitHub](https://github.com/Prats222/switch_backup_revise_files/blob/master/DSA-CSharp-Interview-Tracker/index.html)
- [Vercel deployment](https://vercel.com/prats222s-projects/csharp-dsa-interview-tracker)
- [Source HTML](https://github.com/Prats222/switch_backup_revise_files/blob/master/DSA-CSharp-Interview-Tracker/index.html).

## Features

- 55 questions: 50 core questions plus 5 optional bonus questions.
- Topic, difficulty, completion-status and text filters.
- Direct LeetCode problem links.
- YouTube search links targeted to C# explanations for each question.
- Progress dashboard and browser-local progress persistence.
- Responsive layout for desktop and mobile.

## Progress storage

Completion state is stored in the browser using `localStorage`. It is specific to the browser/device where you use the page. Clearing browser site data will clear this progress.

## Run locally

Download `index.html` and open it in a modern browser. No build step, framework, server, or dependencies are required.

## GitHub Pages

To host this as a GitHub Pages site, configure the repository's **Settings → Pages** to deploy from the `master` branch and the repository root, then place/copy the tracker as the root `index.html` (or configure a `docs` folder as the publishing source and put the file there).
