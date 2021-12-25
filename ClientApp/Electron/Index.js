const { app, BrowserWindow } = require('electron')

function refresh() 
{  

}

function createWindow() {
  const win = new BrowserWindow({fullscreen: true})
    win.loadURL('https://localhost:5001')

    const contents = win.webContents

    contents.on('did-finish-load' , () => {
      console.log("finish")
    })
    
    contents.on('did-fail-load' , () => {
      console.log("fail")
      win.loadURL('https://localhost:5001')
    })
    
    console.log(contents)
}

app.commandLine.appendSwitch('ignore-certificate-errors')

app.on('ready', () => { 
  createWindow()
})
