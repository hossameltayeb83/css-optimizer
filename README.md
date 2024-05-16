
<div align="center">

  <h3 align="center">Css Optimizer</h3>

  <p align="center">
   Remove Unnecessary css styles
  </p>
</div>

# How It Works

1- **Start By Cloning the program or download the zip files and run the program**

2- **Provide the project path you want to optimize**

3- **Enter whither you want to exclude some folders inside the project direcotry** 

4- **If yes provide the list of folder names separated by a space E.g. bootstrap debug**

5-**The program will then extract all classes used in html or cshtml files inside the project**
```
<div class="person">
        <div class=" person-details center"></div>
        <div class="center">
          <div class="title">Catch up with me</div>
          <div class="sub">The roof terrace</div>
        </div>
      </div>
```
classes extracted : person, person-details, center, title, sub
<br/>

5- **The program then will extract all css files from the project folder**

6- **The program will check the selecotrs of every block of styles in the css files compared to the extracted classes from html files**

7- **If the selectors are not used or applied the program will remove them with the associated block of styles**
```
1- this will be kept 
.person{
  /*css styles*/
}
2- this will be removed 
.avatar{
  /*css styles*/
}
3- this will will be kept, since a tag selector is applied 
.avatar, p{
  /*css styles*/
}
4- the styles of all these selectors will be removed because none of them are reachable because of the missing class:
  p.avatar
  .avatar.person
  .avatar .person
  .avatar > .person
  .avatar + .person
5- if selectors are separated by a comma(,) each comma separated selector will be evaluated separately
  p.avatar, .person-details /* will be kept: styles for person-details are needed*/
  .avatar , .avatar2 /* will be removed: both selectors are not used*/
```
8- **The program will replace old css files with the newly optimized one-it's advisible to test the output first on a copy of the project-**
