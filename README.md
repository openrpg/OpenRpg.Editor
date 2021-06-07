# OpenRpg.Editor
Hey! So the editor itself is a work in progress, but should work fine for basic item, class, race and maybe quest creation.

![example2](https://i.imgur.com/ShxusWF.png)

## What is it?

It is basically an out the box editor for you to create your own content for use in your games with OpenRpg.

It is broken up a bit so you can take bits of it and use it to make your own custom editor which has more of a focus on your scenario, but you can just as easily use it out of the box.

It is currently focused on managing items, classes, races but more will be added as we go.

### The parts of it

There are a couple of projects within this solution and they all do slightly different things:

#### `OpenRpg.Editor.Infrastructure`

This contains the core logic and *infrastructure* classes for the editor to use, such as pipelines to load and save data as well as other services and helpers.

#### `OpenRpg.Editor`

On top of that we have the core *Editor* library which is basically a suite of reusable components with some default pages that you can consume in your own blazor applications.

#### `OpenRpg.Editor.App` *(Recommended Startup App)*

This is an application version of the editor which is run within a .net web view, so it can access local file systems and other things the browser cant.

It is also worth noting that this project contains a `Content` folder which has all the default locale and game information files, so you can edit them as much as you want and copy and paste them out to your game when you are done.

#### `OpenRpg.Editor.Web`

For the most part this can be ignored for now, it is a placeholder for a web based version that we would like to push out at a later date so anyone can consume it from their browser.

## How to use it?

For now just clone the repo, restore the nuget packages and run the `OpenRpg.Editor.App`, this will run the editor as an app and will output all changes to the local `Content` folder.

Currently its all a bit prototypey so some things may blow up, and its recommended that you take frequent back ups of your content folder when you are done editing.