import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { environment } from "src/environments/environment";
import { ModuleService } from "../../home/_services/module.service";

@Component({
    selector: 'sh-modules-settings',
    templateUrl: './modules-settings.component.html',
    styleUrls: ['./modules-settings.component.scss']
})

export class ModulesSettingsComponent implements OnInit{
    isLoading: boolean;
    hasError: boolean;
    modules : any[] = []

    constructor(public translate : TranslateService , private moduleService : ModuleService){
        this.isLoading = false
        this.hasError = false
    }

    ngOnInit(){
        this.moduleService.fetchModules()

        this.moduleService._modules$.subscribe(m => {
            this.modules = m
        })
    }
}
