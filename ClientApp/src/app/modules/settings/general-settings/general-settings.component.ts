import { Component, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { LayoutService } from "src/app/pages/_layout/layout.service";

@Component({
    selector : 'sh-general-settings' ,
    templateUrl : './general-settings.component.html' ,
    styleUrls : [ './general-settings.component.scss' ]
})

export class GeneralSettingsComponent implements OnInit{
    config : any;
    constructor(private layoutService : LayoutService , private router: Router){}

    ngOnInit(){
        this.config = this.layoutService.getConfig()
    }

    setConfig(name , type){
        this.config.theme.name = name
        this.config.theme.type = type

        this.layoutService.setConfig(this.config)
        // 
    }

    setAndRedirect(name , type){
        this.setConfig(name , type)
        this.router.navigateByUrl('/')
        // 
        setTimeout(() => document.location.reload() , 300)
    }
}