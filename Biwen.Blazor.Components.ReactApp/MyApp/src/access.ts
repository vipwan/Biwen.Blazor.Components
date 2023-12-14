/*
 * @Author: 万雅虎
 * @Date: 2023-12-11 16:26:19
 * @LastEditTime: 2023-12-11 16:41:05
 * @LastEditors: 万雅虎
 * @Description:
 * @FilePath: \MyApp\src\access.ts
 * vipwan@sina.com © 万雅虎
 */
/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(initialState: { currentUser?: API.CurrentUser } | undefined) {
  const { currentUser } = initialState ?? {};
  return {
    canAdmin: true,//currentUser && currentUser.access === 'admin',
  };
}
