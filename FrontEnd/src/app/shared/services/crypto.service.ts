import { Injectable } from '@angular/core'
import *  as CryptoJS from 'crypto-js'
// Custom
import { SessionStorageService } from './session-storage.service'

@Injectable({ providedIn: 'root' })

export class CryptoService {

    private key = '123'

    constructor(private sessionStorageService: SessionStorageService) { }

    public encrypt(txt: string): string {
        return CryptoJS.AES.encrypt(txt.toString(), this.key).toString()
    }

    public decrypt(txtToDecrypt: string): string {
        return CryptoJS.AES.decrypt(txtToDecrypt, this.key).toString(CryptoJS.enc.Utf8)
    }

}
