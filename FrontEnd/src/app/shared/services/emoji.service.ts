import { Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })

export class EmojiService {

    public getEmoji(emoji: string): string {
        switch (emoji) {
            case 'active': return '🟢 '
            case 'blue-box': return '🟦'
            case 'edit': return '✒️'
            case 'empty': return ' '
            case 'error': return '❌'
            case 'green-box': return '🟩'
            case 'linktwist': return '⚡'
            case 'notActive': return '☢️ '
            case 'null': return '🚫'
            case 'ok': return '✔️'
            case 'red-box': return '🟥'
            case 'remarks': return '🔔'
            case 'warning': return '⚠️'
            case 'wildcard': return '⭐'
            case 'yellow-box': return '🟨'
        }

    }

}
